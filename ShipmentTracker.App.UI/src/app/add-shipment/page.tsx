"use client";
import {
  Box,
  Button,
  MenuItem,
  TextField,
  Typography,
  Select,
  FormControl,
  InputLabel,
} from "@mui/material";
import { useState, useEffect } from "react";
import api from "../../../lib/api";
import { AddShipmentsRequest, Carrier } from "../../../types";
import { useRouter } from "next/navigation";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import dayjs from "dayjs";
import RefreshIcon from "@mui/icons-material/Refresh";

export default function AddShipment() {
  const [form, setForm] = useState<AddShipmentsRequest>({
    origin: "",
    destination: "",
    carrierId: null,
    shipmentDate: "",
    estimatedDeliveryDate: "",
  });

  const [carriers, setCarriers] = useState<Carrier[]>([]);
  const router = useRouter();

  useEffect(() => {
    api.get("/carriers").then((res) => setCarriers(res.data));
  }, []);

  const handleSubmit = async () => {
    await api.post("/shipments", {
      ...form,
    });
    router.push("/dashboard");
  };

  const resetForm = () => {
    setForm({
      origin: "",
      destination: "",
      carrierId: null,
      shipmentDate: "",
      estimatedDeliveryDate: "",
    });
  };

  return (
    <Box
      sx={{
        p: 3,
        display: "flex",
        flexDirection: "column",
        width: "100%",
        maxWidth: 600,
        margin: "auto",
      }}
    >
      <Typography variant="h4" gutterBottom textAlign={"center"}>
        Add Shipment
      </Typography>

      <Box sx={{ display: "flex", flexDirection: "column", gap: 2, mt: 2 }}>
        <TextField
          label="Origin"
          value={form.origin}
          onChange={(e) => setForm({ ...form, origin: e.target.value })}
        />
        <TextField
          label="Destination"
          value={form.destination}
          onChange={(e) => setForm({ ...form, destination: e.target.value })}
        />
        <FormControl>
          <InputLabel>Carrier</InputLabel>
          <Select
            value={form.carrierId}
            onChange={(e) =>
              setForm({ ...form, carrierId: Number(e.target.value) })
            }
            label="Carrier"
          >
            {carriers.map((c) => (
              <MenuItem key={c.id} value={c.id}>
                {c.name}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
        <LocalizationProvider dateAdapter={AdapterDayjs}>
          <DatePicker
            label="Shipment Date"
            format="MM-DD-YYYY"
            value={form.shipmentDate ? dayjs(form.shipmentDate) : null}
            onChange={(newValue) =>
              setForm({
                ...form,
                shipmentDate: newValue ? newValue.format("YYYY-MM-DD") : "",
              })
            }
          />
          <DatePicker
            label="Estimated Delivery"
            format="MM-DD-YYYY"
            value={
              form.estimatedDeliveryDate
                ? dayjs(form.estimatedDeliveryDate)
                : null
            }
            onChange={(newValue) =>
              setForm({
                ...form,
                estimatedDeliveryDate: newValue
                  ? newValue.format("YYYY-MM-DD")
                  : "",
              })
            }
          />
        </LocalizationProvider>
      </Box>
      <Box
        sx={{
          display: "flex",
          flexDirection: "row",
          justifyContent: "space-between",
          gap: 2,
          mt: 2,
        }}
      >
        <Button
          variant="contained"
          onClick={resetForm}
          startIcon={<RefreshIcon />}
          fullWidth
        >
          Reset
        </Button>
        <Button variant="contained" onClick={handleSubmit} fullWidth>
          Submit
        </Button>
        <Button
          variant="contained"
          onClick={() => {
            router.push("/dashboard");
          }}
          startIcon={<ArrowBackIcon />}
          fullWidth
        >
          Back
        </Button>
      </Box>
    </Box>
  );
}
