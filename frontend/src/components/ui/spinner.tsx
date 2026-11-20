import { cn } from "cn"
import { Spinner as SpinnerIcon } from "@phosphor-icons/react/Spinner"

function Spinner({ className, ...props }: React.ComponentProps<"svg">) {
  return (
    <SpinnerIcon data-slot="spinner" role="status" aria-label="Loading" className={cn("size-4 animate-spin", className)} {...props} />
  )
}

export { Spinner }
