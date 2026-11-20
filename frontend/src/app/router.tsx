import { useMemo } from "react"
import { createBrowserRouter, RouterProvider, Navigate } from "react-router"

import { AuthLayout } from "@/components/layouts/auth-layout"
import { ProtectedRoute } from "@/components/protected-route"
import { paths } from "@/config/paths"
import LoginPage from "@/pages/auth/login-page"
import SignupPage from "@/pages/auth/signup-page"
import DashboardPage from "@/pages/dashboard-page"

export const createAppRouter = () =>
  createBrowserRouter([
    {
      element: <ProtectedRoute />,
      children: [
        {
          path: paths.home,
          element: <DashboardPage />,
        },
      ],
    },
    {
      element: <AuthLayout />,
      children: [
        {
          path: paths.auth.signup,
          element: <SignupPage />,
        },
        {
          path: paths.auth.login,
          element: <LoginPage />,
        },
      ],
    },
    {
      path: "*",
      element: <Navigate to={paths.home} replace />,
    },
  ])

export const AppRouter = () => {
  const router = useMemo(() => createAppRouter(), [])

  return <RouterProvider router={router} />
}
