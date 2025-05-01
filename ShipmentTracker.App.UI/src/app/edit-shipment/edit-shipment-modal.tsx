import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import { FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import { getShipmentStatus } from "../../../data/shipment-status";
import api from "../../../lib/api";

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
  const [selectedStatus, setSelectedStatus] = React.useState<number>(statusId);
  const statusOptions = getShipmentStatus();

  React.useEffect(() => {
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
    } else {
      console.error("Error updating shipment status", response);
    }
  };

  return (
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
  );
};

export default EditShipmentModal;
