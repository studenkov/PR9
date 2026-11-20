import { useState } from "react"
import { useNavigate, Link } from "react-router"
import { WarningCircle } from "@phosphor-icons/react/WarningCircle"
import { CheckCircle } from "@phosphor-icons/react/CheckCircle"

import {
  Alert,
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

export function SignupForm({ ...props }: React.ComponentProps<typeof Card>) {
  const navigate = useNavigate()
  const [username, setUsername] = useState("")
  const [password, setPassword] = useState("")
  const [confirmPassword, setConfirmPassword] = useState("")
  const [isLoading, setIsLoading] = useState(false)
  const [errorMessage, setErrorMessage] = useState<string | null>(null)
  const [successMessage, setSuccessMessage] = useState<string | null>(null)

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault()
    setErrorMessage(null)
    setSuccessMessage(null)

    if (password !== confirmPassword) {
      setErrorMessage("Пароли не совпадают")
      return
    }

    if (password.length < 6) {
      setErrorMessage("Пароль должен содержать не менее 6 символов")
      return
    }

    setIsLoading(true)
    const res = await authApi.register({ username, password })
    setIsLoading(false)

    if (res.ok) {
      setSuccessMessage(
        "Аккаунт успешно создан! Перенаправление на страницу входа..."
      )
      setTimeout(() => {
        navigate(paths.auth.login)
      }, 1500)
      return
    }

    if (res.isCorsOrNetworkError) {
      setErrorMessage(
        "Ошибка сети / CORS: сервер API недоступен или заблокирован браузером."
      )
    } else {
      setErrorMessage(res.error || `Ошибка регистрации (HTTP ${res.status})`)
    }
  }

  return (
    <Card {...props}>
      <CardHeader>
        <CardTitle>Создайте аккаунт</CardTitle>
        <CardDescription>
          Введите информацию ниже, чтобы зарегистрироваться в системе
        </CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit}>
          <FieldGroup className="gap-4">
            {errorMessage && (
              <Alert variant="destructive">
                <WarningCircle className="size-4" />
                <AlertDescription>{errorMessage}</AlertDescription>
              </Alert>
            )}

            {successMessage && (
              <Alert className="border-green-500/30 text-green-700 dark:text-green-400 bg-green-500/10">
                <CheckCircle className="size-4 text-green-600 dark:text-green-400" />
                <AlertDescription>{successMessage}</AlertDescription>
              </Alert>
            )}

            <Field>
              <FieldLabel htmlFor="username">Логин</FieldLabel>
              <Input
                id="username"
                type="text"
                autoComplete="username"
                placeholder="johndoe"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                disabled={isLoading}
                required
              />
              <FieldDescription>
                Логин, который будет использоваться для входа на сайт
              </FieldDescription>
            </Field>

            <Field>
              <FieldLabel htmlFor="new-password">Пароль</FieldLabel>
              <Input
                id="new-password"
                type="password"
                autoComplete="new-password"
                placeholder="Пароль"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                disabled={isLoading}
                required
              />
              <FieldDescription>
                Пароль должен быть минимум 6 символов
              </FieldDescription>
            </Field>

            <Field>
              <FieldLabel htmlFor="confirm-password">
                Подтвердите пароль
              </FieldLabel>
              <Input
                id="confirm-password"
                type="password"
                autoComplete="new-password"
                placeholder="Подтвердите пароль"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                disabled={isLoading}
                required
              />
              <FieldDescription>Пароли должны совпадать</FieldDescription>
            </Field>

            <FieldGroup>
              <Field>
                <Button type="submit" disabled={isLoading} className="w-full">
                  {isLoading ? (
                    <span className="flex items-center gap-2">
                      <Spinner className="size-4" />
                      Создание...
                    </span>
                  ) : (
                    "Создать аккаунт"
                  )}
                </Button>
                <FieldDescription className="px-6 text-center">
                  Уже есть аккаунт?{" "}
                  <Link
                    to={paths.auth.login}
                    className="underline underline-offset-4"
                  >
                    Войти
                  </Link>
                </FieldDescription>
              </Field>
            </FieldGroup>
          </FieldGroup>
        </form>
      </CardContent>
    </Card>
  )
}
