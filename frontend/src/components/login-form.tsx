import { useState } from "react"
import { useNavigate, Link } from "react-router"
import { WarningCircle } from "@phosphor-icons/react/WarningCircle"
import { cn } from "cn"

import {
  Alert,
  AlertTitle,
  AlertDescription,
} from "@/components/ui/alert"
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
  FieldDescription,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import { Spinner } from "@/components/ui/spinner"
import { paths } from "@/config/paths"
import { authApi } from "@/lib/api"

export function LoginForm({
  className,
  ...props
}: React.ComponentProps<"div">) {
  const navigate = useNavigate()
  const [username, setUsername] = useState("")
  const [password, setPassword] = useState("")
  const [isLoading, setIsLoading] = useState(false)
  const [errorMessage, setErrorMessage] = useState<string | null>(null)
  const [errorStatus, setErrorStatus] = useState<number | null>(null)

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault()
    setIsLoading(true)
    setErrorMessage(null)
    setErrorStatus(null)

    const res = await authApi.login({ username, password })
    setIsLoading(false)

    if (res.ok) {
      navigate(paths.home)
      return
    }

    setErrorStatus(res.status)
    if (res.isRateLimited) {
      setErrorMessage("429 Too Many Requests: сработал Rate Limiting. Превышен лимит попыток входа.")
    } else if (res.isUnauthorized) {
      setErrorMessage("401 Unauthorized: неверный логин или пароль.")
    } else if (res.isCorsOrNetworkError) {
      setErrorMessage("Ошибка сети / CORS: сервер API недоступен или заблокирован браузером.")
    } else {
      setErrorMessage(res.error || `Ошибка авторизации (HTTP ${res.status})`)
    }
  }

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader>
          <CardTitle>Войдите в аккаунт</CardTitle>
          <CardDescription>
            Введите логин и пароль для получения JWT токена
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit}>
            <FieldGroup className="gap-4">
              {errorMessage && (
                <Alert variant="destructive">
                  <WarningCircle className="size-4" />
                  <AlertTitle>
                    {errorStatus ? `Ошибка HTTP ${errorStatus}` : "Ошибка запроса"}
                  </AlertTitle>
                  <AlertDescription>{errorMessage}</AlertDescription>
                </Alert>
              )}

              <Field data-invalid={!!errorMessage}>
                <FieldLabel htmlFor="username">Логин</FieldLabel>
                <Input
                  id="username"
                  type="text"
                  autoComplete="username"
                  placeholder="johndoe"
                  value={username}
                  onChange={(e) => {
                    setUsername(e.target.value)
                    if (errorMessage) {
                      setErrorMessage(null)
                      setErrorStatus(null)
                    }
                  }}
                  disabled={isLoading}
                  aria-invalid={!!errorMessage}
                  required
                />
              </Field>

              <Field data-invalid={!!errorMessage}>
                <div className="flex items-center">
                  <FieldLabel htmlFor="password">Пароль</FieldLabel>
                </div>
                <Input
                  id="password"
                  type="password"
                  autoComplete="current-password"
                  placeholder="Пароль"
                  value={password}
                  onChange={(e) => {
                    setPassword(e.target.value)
                    if (errorMessage) {
                      setErrorMessage(null)
                      setErrorStatus(null)
                    }
                  }}
                  disabled={isLoading}
                  aria-invalid={!!errorMessage}
                  required
                />
              </Field>

              <Field className="gap-3 pt-1">
                <Button type="submit" disabled={isLoading} className="w-full">
                  {isLoading ? (
                    <span className="flex items-center gap-2">
                      <Spinner className="size-4" />
                      Вход...
                    </span>
                  ) : (
                    "Войти"
                  )}
                </Button>
                <FieldDescription className="text-center">
                  Нет аккаунта?{" "}
                  <Link to={paths.auth.signup} className="underline underline-offset-4">
                    Зарегистрируйтесь
                  </Link>
                </FieldDescription>
              </Field>
            </FieldGroup>
          </form>
        </CardContent>
      </Card>
    </div>
  )
}
