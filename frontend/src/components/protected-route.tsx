import { Navigate, Outlet } from "react-router"
import { getToken } from "@/lib/api"
import { paths } from "@/config/paths"

export function ProtectedRoute() {
  const token = getToken()

  if (!token) {
    return <Navigate to={paths.auth.login} replace />
  }

  return <Outlet />
}
