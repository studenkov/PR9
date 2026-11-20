import { useState } from "react"
import { useNavigate } from "react-router"

import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import {
  Field,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import { Badge } from "@/components/ui/badge"
import { Spinner } from "@/components/ui/spinner"
import { paths } from "@/config/paths"
import {
  getCurrentUser,
  getToken,
  authApi,
  apiRequest,
  API_BASE_URL,
  type JwtUser,
  type ApiResponse,
} from "@/lib/api"

interface OrderResponse {
  id: number
  userId: number
  dateOrder: string
}

export default function DashboardPage() {
  const navigate = useNavigate()
  const [user] = useState<JwtUser | null>(() => getCurrentUser())
  const [token] = useState<string>(() => getToken() || "")
  const [orderId, setOrderId] = useState<string>("1")
  const [isLoading, setIsLoading] = useState<boolean>(false)
  const [isCopied, setIsCopied] = useState<boolean>(false)
  const [lastResponse, setLastResponse] = useState<ApiResponse | null>(null)

  const username = user?.username ?? "—"
  const userId = user?.id ?? "—"
  const userRole = user?.role ?? "—"
  const isAdmin = user?.role === "Admin"
  const expirationTime = user?.exp
    ? new Date(user.exp * 1000).toLocaleTimeString()
    : "—"
  const displayToken = token || "Токен отсутствует"
  const copyButtonLabel = isCopied ? "Скопировано" : "Скопировать токен"
  const responseStatusText = lastResponse
    ? lastResponse.status > 0
      ? `HTTP ${lastResponse.status}`
      : "Network / CORS Error"
    : null
  const responseBodyText = lastResponse
    ? JSON.stringify(lastResponse.data ?? lastResponse.error, null, 2)
    : null

  const handleLogout = () => {
    authApi.logout()
    navigate(paths.auth.login)
  }

  const handleCopyToken = () => {
    if (!token) return
    navigator.clipboard.writeText(token)
    setIsCopied(true)
    setTimeout(() => setIsCopied(false), 2000)
  }

  const handleGetOrderById = async () => {
    setIsLoading(true)
    const res = await apiRequest(`/api/order/${orderId}`, {
      method: "GET",
    })
    setLastResponse(res)
    setIsLoading(false)
  }

  const handleCreateMyOrder = async () => {
    if (!user?.id) return
    setIsLoading(true)
    const today = new Date().toISOString().split("T")[0]
    const res = await apiRequest<OrderResponse>("/api/order", {
      method: "POST",
      body: JSON.stringify({
        userId: parseInt(user.id, 10),
        dateOrder: today,
      }),
    })
    if (res.ok && res.data?.id) {
      setOrderId(res.data.id.toString())
    }
    setLastResponse(res)
    setIsLoading(false)
  }

  const handleDeleteOrder = async () => {
    setIsLoading(true)
    const res = await apiRequest(`/api/order/${orderId}`, {
      method: "DELETE",
    })
    setLastResponse(res)
    setIsLoading(false)
  }

  const handleGetAllOrders = async () => {
    setIsLoading(true)
    const res = await apiRequest("/api/order", {
      method: "GET",
    })
    setLastResponse(res)
    setIsLoading(false)
  }

  const handleRequestWithoutToken = async () => {
    setIsLoading(true)
    const res = await apiRequest("/api/order", {
      method: "GET",
      skipToken: true,
    })
    setLastResponse(res)
    setIsLoading(false)
  }

  const handleTestRateLimit = async () => {
    setIsLoading(true)
    let finalRes: ApiResponse | null = null
    for (let i = 0; i < 6; i++) {
      finalRes = await apiRequest("/api/auth/login", {
        method: "POST",
        body: JSON.stringify({
          login: "test_probe",
          password: "wrong_password",
        }),
      })
      if (finalRes.status === 429) {
        break
      }
    }
    setLastResponse(finalRes)
    setIsLoading(false)
  }

  return (
    <div className="container mx-auto max-w-4xl space-y-6 p-4 md:p-8">
      <div className="flex items-center justify-between border-b pb-4">
        <div>
          <h1 className="text-xl font-semibold tracking-tight">Панель управления</h1>
          <p className="text-xs text-muted-foreground font-mono mt-0.5">
            {API_BASE_URL}
          </p>
        </div>
        <Button variant="outline" size="sm" onClick={handleLogout}>
          Выйти
        </Button>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        <Card className="flex flex-col justify-between">
          <CardHeader>
            <CardTitle>Профиль пользователя</CardTitle>
            <CardDescription>Данные текущей сессии</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-[140px_1fr] items-center gap-y-3 text-sm">
              <span className="text-muted-foreground">Логин</span>
              <span className="font-medium text-right">{username}</span>

              <span className="text-muted-foreground">ID пользователя</span>
              <span className="font-mono text-xs text-right">{userId}</span>

              <span className="text-muted-foreground">Роль</span>
              <span className="text-right">
                <Badge variant="outline">{userRole}</Badge>
              </span>

              <span className="text-muted-foreground">Истекает</span>
              <span className="font-mono text-xs text-right">{expirationTime}</span>
            </div>
          </CardContent>
        </Card>

        <Card className="flex flex-col justify-between">
          <CardHeader>
            <CardTitle>Access Token</CardTitle>
            <CardDescription>JWT для авторизации запросов</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="max-h-24 overflow-y-auto rounded-md border bg-muted/40 p-2.5 font-mono text-[11px] break-all select-all text-muted-foreground">
              {displayToken}
            </div>
            <Button
              variant="outline"
              size="sm"
              onClick={handleCopyToken}
              disabled={!token}
            >
              {copyButtonLabel}
            </Button>
          </CardContent>
        </Card>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        <Card className="flex flex-col justify-between">
          <CardHeader>
            <CardTitle>Заказы</CardTitle>
            <CardDescription>Проверка доступа (Resource и Role policy)</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <FieldGroup>
              <Field>
                <FieldLabel htmlFor="order-id">ID заказа</FieldLabel>
                <Input
                  id="order-id"
                  value={orderId}
                  onChange={(e) => setOrderId(e.target.value)}
                  placeholder="1"
                />
              </Field>
            </FieldGroup>

            <div className="flex flex-wrap gap-2 pt-1">
              <Button
                variant="outline"
                size="sm"
                onClick={handleGetOrderById}
                disabled={isLoading}
              >
                Просмотреть
              </Button>
              <Button
                variant="outline"
                size="sm"
                onClick={handleCreateMyOrder}
                disabled={isLoading}
              >
                Создать свой заказ
              </Button>
              <Button
                variant="outline"
                size="sm"
                onClick={handleDeleteOrder}
                disabled={isLoading}
              >
                Удалить
              </Button>
              {isAdmin && (
                <Button
                  variant="ghost"
                  size="sm"
                  onClick={handleGetAllOrders}
                  disabled={isLoading}
                >
                  Все заказы (Admin)
                </Button>
              )}
            </div>
          </CardContent>
        </Card>

        <Card className="flex flex-col justify-between">
          <CardHeader>
            <CardTitle>Безопасность и лимиты</CardTitle>
            <CardDescription>Проверка статус-кодов</CardDescription>
          </CardHeader>
          <CardContent className="space-y-3">
            <Button
              variant="outline"
              size="sm"
              className="w-full justify-start"
              onClick={handleRequestWithoutToken}
              disabled={isLoading}
            >
              Запрос без токена (401)
            </Button>
            <Button
              variant="outline"
              size="sm"
              className="w-full justify-start"
              onClick={handleTestRateLimit}
              disabled={isLoading}
            >
              Тест Rate Limiting (429)
            </Button>
          </CardContent>
        </Card>
      </div>

      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-3">
          <CardTitle>Ответ сервера</CardTitle>
          <div className="flex items-center gap-2">
            {isLoading && <Spinner className="size-4" />}
            {responseStatusText && (
              <Badge variant="outline" className="font-mono">
                {responseStatusText}
              </Badge>
            )}
          </div>
        </CardHeader>
        <CardContent>
          {responseBodyText ? (
            <pre className="max-h-56 overflow-y-auto rounded-md border bg-muted/40 p-3 font-mono text-xs">
              {responseBodyText}
            </pre>
          ) : (
            <p className="text-xs text-muted-foreground">Запросов пока не было.</p>
          )}
        </CardContent>
      </Card>
    </div>
  )
}
