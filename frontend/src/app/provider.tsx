import { ThemeProvider } from "@/components/theme-provider"
import { Spinner } from "@/components/ui/spinner"
import { Suspense } from "react"

type AppProviderProps = {
  children: React.ReactNode
}

export const AppProvider = ({ children }: AppProviderProps) => {
  return (
    <Suspense
      fallback={
        <div className="flex h-screen w-screen items-center justify-center">
          <Spinner className="size-8" />
        </div>
      }
    >
      <ThemeProvider>{children}</ThemeProvider>
    </Suspense>
  )
}
