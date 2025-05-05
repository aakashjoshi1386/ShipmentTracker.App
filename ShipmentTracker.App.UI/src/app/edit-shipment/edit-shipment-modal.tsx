import { useEffect, useState } from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import { FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import { getShipmentStatus } from "../../../data/shipment-status";
import api from "../../../lib/api";
import Toast, { ToastProps } from "@/components/ui/toast";

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

interface EditShipmentModalProps {
  open: boolean;
  onClose: () => void;
  onSuccess: () => void;
  id: number;
  statusId: number;
}

const EditShipmentModal: React.FC<EditShipmentModalProps> = ({
  open,
  onClose,
  onSuccess,
  id,
  statusId: statusId,
}) => {
  const [toast, setToast] = useState<ToastProps>({
    message: "",
    severity: "info",
    open: false,
    onClose: () => {},
  });
  const [selectedStatus, setSelectedStatus] = useState<number>(statusId);
  const statusOptions = getShipmentStatus();

  useEffect(() => {
    setSelectedStatus(statusId);
  }, [statusId]);

  const handleSave = async () => {
    const response = await api.put(`/shipments/${id}/status`, {
      id,
      statusId: selectedStatus,
    });
    if (response) {
      onSuccess();
      onClose();
      setToast({
        message: "Shipment updated successfully",
        severity: "success",
        open: true,
        onClose: () => setToast((prev) => ({ ...prev, open: false })),
      });
    } else {
      setToast({
        message: "Error updating shipment status",
        severity: "error",
        open: true,
        onClose: () => setToast((prev) => ({ ...prev, open: false })),
      });
    }
  };

  return (
    <>
      <Modal
        open={open}
        onClose={onClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={style}>
          <Typography id="modal-modal-title" variant="h6" component="h2">
            Edit Shipment Status
          </Typography>
          <FormControl fullWidth sx={{ mt: 2 }}>
            <InputLabel id="status-label">Status</InputLabel>
            <Select
              labelId="status-label"
              value={selectedStatus}
              onChange={(e) => setSelectedStatus(Number(e.target.value))}
              label="Status"
            >
              {statusOptions.map((s) => (
                <MenuItem key={s.id} value={s.id}>
                  {s.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <Box sx={{ display: "flex", justifyContent: "flex-end", mt: 3 }}>
            <Button onClick={onClose} sx={{ mr: 1 }}>
              Cancel
            </Button>
            <Button variant="contained" onClick={handleSave}>
              Save
            </Button>
          </Box>
        </Box>
      </Modal>
      {toast.open && (
        <Toast
          message={toast.message}
          onClose={toast.onClose}
          open={toast.open}
          severity={toast.severity}
        />
      )}
    </>
  );
};

export default EditShipmentModal;
