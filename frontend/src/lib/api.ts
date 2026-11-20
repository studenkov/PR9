export const API_BASE_URL: string =
  (import.meta.env.VITE_API_URL as string | undefined)?.replace(/\/+$/, "") ||
  "http://localhost:5000"

const TOKEN_KEY = "access_token"

export interface JwtUser {
  id: string
  username: string
  role: string
  exp?: number
  raw: Record<string, unknown>
}

export interface ApiResponse<T = unknown> {
  data: T | null
  status: number
  ok: boolean
  error: string | null
  isCorsOrNetworkError?: boolean
  isRateLimited?: boolean
  isUnauthorized?: boolean
  isForbidden?: boolean
}

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY)
}

export function setToken(token: string): void {
  localStorage.setItem(TOKEN_KEY, token)
}

export function removeToken(): void {
  localStorage.removeItem(TOKEN_KEY)
}

export function parseJwt(token: string): JwtUser | null {
  try {
    const parts = token.split(".")
    if (parts.length !== 3) return null

    const base64Url = parts[1]
    const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/")
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split("")
        .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
        .join("")
    )
    const raw = JSON.parse(jsonPayload) as Record<string, unknown>

    const id =
      (raw[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
      ] as string) ||
      (raw["nameid"] as string) ||
      (raw["sub"] as string) ||
      (raw["id"] as string) ||
      (raw["Id"] as string) ||
      ""

    const role =
      (raw[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ] as string) ||
      (raw["role"] as string) ||
      (raw["Role"] as string) ||
      ""

    const username =
      (raw[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
      ] as string) ||
      (raw["unique_name"] as string) ||
      (raw["name"] as string) ||
      (raw["username"] as string) ||
      (raw["Username"] as string) ||
      ""

    const exp = typeof raw["exp"] === "number" ? raw["exp"] : undefined

    return { id, username, role, exp, raw }
  } catch {
    return null
  }
}

export function getCurrentUser(): JwtUser | null {
  const token = getToken()
  if (!token) return null
  return parseJwt(token)
}

export interface RequestOptions extends RequestInit {
  skipToken?: boolean
}

export async function apiRequest<T = unknown>(
  endpoint: string,
  options: RequestOptions = {}
): Promise<ApiResponse<T>> {
  const url = endpoint.startsWith("http")
    ? endpoint
    : `${API_BASE_URL}${endpoint.startsWith("/") ? "" : "/"}${endpoint}`

  const headers = new Headers(options.headers || {})

  if (!headers.has("Content-Type") && options.body && typeof options.body === "string") {
    headers.set("Content-Type", "application/json")
  }

  const token = getToken()
  if (token && !options.skipToken && !headers.has("Authorization")) {
    headers.set("Authorization", `Bearer ${token}`)
  }

  try {
    const res = await fetch(url, {
      ...options,
      headers,
    })

    let data: T | null = null
    let error: string | null = null

    const contentType = res.headers.get("content-type")
    if (contentType && contentType.includes("application/json")) {
      try {
        data = (await res.json()) as T
      } catch {
        data = null
      }
    } else {
      const text = await res.text()
      data = (text as unknown) as T
    }

    if (!res.ok) {
      if (typeof data === "object" && data !== null) {
        const errorObj = data as Record<string, unknown>
        error =
          (errorObj["message"] as string) ||
          (errorObj["title"] as string) ||
          (errorObj["error"] as string) ||
          res.statusText ||
          `HTTP Error ${res.status}`
      } else if (typeof data === "string" && data.length > 0) {
        error = data
      } else {
        error = res.statusText || `HTTP Error ${res.status}`
      }
    }

    return {
      data: res.ok ? data : null,
      status: res.status,
      ok: res.ok,
      error,
      isRateLimited: res.status === 429,
      isUnauthorized: res.status === 401,
      isForbidden: res.status === 403,
    }
  } catch (err) {
    const isNetworkOrCors =
      err instanceof TypeError &&
      (err.message.includes("fetch") || err.message.includes("NetworkError"))

    return {
      data: null,
      status: 0,
      ok: false,
      error: isNetworkOrCors
        ? "Сетевая ошибка или блокировка CORS (сервер недоступен либо заблокирован браузером)"
        : err instanceof Error
        ? err.message
        : "Неизвестная ошибка запроса",
      isCorsOrNetworkError: true,
    }
  }
}

export interface AuthResponse {
  token?: string
  accessToken?: string
  access_token?: string
  message?: string
}

export const authApi = {
  async login(payload: { username: string; password: string }): Promise<ApiResponse<AuthResponse>> {
    const res = await apiRequest<AuthResponse>("/api/auth/login", {
      method: "POST",
      body: JSON.stringify({
        login: payload.username,
        password: payload.password,
      }),
    })

    const token =
      res.data?.accessToken || res.data?.token || res.data?.access_token

    if (res.ok && token) {
      setToken(token)
    }

    return res
  },

  async register(payload: { username: string; password: string }): Promise<ApiResponse<AuthResponse>> {
    return apiRequest<AuthResponse>("/api/auth/register", {
      method: "POST",
      body: JSON.stringify({
        login: payload.username,
        password: payload.password,
      }),
    })
  },

  logout(): void {
    removeToken()
  },
}
