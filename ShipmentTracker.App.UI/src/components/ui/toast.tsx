import { Alert } from "@mui/material";
import Snackbar, { SnackbarCloseReason } from "@mui/material/Snackbar";

export interface ToastProps {
  message: string;
  severity: "success" | "error" | "warning" | "info";
  open: boolean;
  onClose: (value: boolean) => void;
}

export const Toast: React.FC<ToastProps> = ({
  message,
  severity,
  open,
  onClose,
}: {
  message: string;
  severity: "success" | "error" | "warning" | "info";
  open: boolean;
  onClose: (value: boolean) => void;
}) => {
  const handleClose = (
    event: React.SyntheticEvent | Event,
    reason?: SnackbarCloseReason
  ) => {
    if (reason === "clickaway") {
      return;
    }

    onClose(true);
  };
  return (
    <>
      <Snackbar
        open={open}
        anchorOrigin={{ vertical: "top", horizontal: "right" }}
        autoHideDuration={5000}
        onClose={handleClose}
      >
        <Alert severity={`${severity}`} onClose={onClose}>
          {message}
        </Alert>
      </Snackbar>
    </>
  );
};

export default Toast;
