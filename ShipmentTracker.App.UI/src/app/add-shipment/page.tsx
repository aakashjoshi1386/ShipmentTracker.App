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
  FormHelperText,
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
import Toast, { ToastProps } from "@/components/ui/toast";
import { validationEngine } from "../../../lib/validationEngine";
import { useFormik } from "formik";

export default function AddShipment() {
  const [toast, setToast] = useState<ToastProps>({
    message: "",
    severity: "info",
    open: false,
    onClose: () => {},
  });

  const [carriers, setCarriers] = useState<Carrier[]>([]);
  const router = useRouter();

  useEffect(() => {
    api.get("/carriers").then((res) => setCarriers(res.data));
  }, []);

  const validationSchema = validationEngine("AddShipment");

  const formik = useFormik<AddShipmentsRequest>({
    initialValues: {
      origin: "",
      destination: "",
      carrierId: null,
      shipmentDate: null,
      estimatedDeliveryDate: null,
    },
    validationSchema,
    onSubmit: (values) => {
      handlesubmit(values);
    },
  });

  const handlesubmit = async (values: AddShipmentsRequest) => {
    try {
      const response = await api.post("/shipments", {
        ...values,
        shipmentDate: values.shipmentDate
          ? dayjs(values.shipmentDate).format("YYYY-MM-DD")
          : null,
        estimatedDeliveryDate: values.estimatedDeliveryDate
          ? dayjs(values.estimatedDeliveryDate).format("YYYY-MM-DD")
          : null,
      });
      if (response) {
        setToast({
          message: "Shipment added successfully",
          severity: "success",
          open: true,
          onClose: () => setToast((prev) => ({ ...prev, open: false })),
        });
        resetForm();
        router.push("/dashboard");
      }
    } catch (err) {
      setToast({
        message: "Failed to add shipment",
        severity: "error",
        open: true,
        onClose: () => setToast((prev) => ({ ...prev, open: false })),
      });
    }
  };

  const resetForm = () => {
    formik.resetForm();
  };

  return (
    <>
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
        <Typography variant="h4" gutterBottom textAlign="center">
          Add Shipment
        </Typography>

        <form onSubmit={formik.handleSubmit}>
          <Box sx={{ display: "flex", flexDirection: "column", gap: 2, mt: 2 }}>
            <TextField
              name="origin"
              label="Origin"
              value={formik.values.origin}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={Boolean(formik.touched.origin && formik.errors.origin)}
              helperText={formik.touched.origin && formik.errors.origin}
            />
            <TextField
              name="destination"
              label="Destination"
              value={formik.values.destination}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={Boolean(
                formik.touched.destination && formik.errors.destination
              )}
              helperText={
                formik.touched.destination && formik.errors.destination
              }
            />
            <FormControl
              error={
                formik.touched.carrierId && formik.errors.carrierId
                  ? true
                  : false
              }
            >
              <InputLabel>Carrier</InputLabel>
              <Select
                name="carrierId"
                label="Carrier"
                value={formik.values.carrierId || ""}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
                error={Boolean(
                  formik.touched.carrierId && formik.errors.carrierId
                )}
              >
                {carriers.map((c) => (
                  <MenuItem key={c.id} value={c.id}>
                    {c.name}
                  </MenuItem>
                ))}
              </Select>
              {formik.touched.carrierId && formik.errors.carrierId && (
                <FormHelperText>
                  {formik.touched.carrierId && formik.errors.carrierId}
                </FormHelperText>
              )}
            </FormControl>
            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <DatePicker
                disablePast
                name="shipmentDate"
                label="Ship Date"
                value={
                  formik.values.shipmentDate
                    ? dayjs(formik.values.shipmentDate)
                    : null
                }
                onChange={(newValue) => {
                  formik.setFieldValue(
                    "shipmentDate",
                    newValue ? newValue.toDate() : null
                  );
                }}
                slotProps={{
                  textField: {
                    name: "shipmentDate",
                    error: Boolean(
                      formik.touched.shipmentDate && formik.errors.shipmentDate
                    ),
                    helperText:
                      formik.touched.shipmentDate && formik.errors.shipmentDate,
                  },
                }}
              />
              <DatePicker
                disablePast
                name="estimatedDeliveryDate"
                label="ETA"
                value={
                  formik.values.estimatedDeliveryDate
                    ? dayjs(formik.values.estimatedDeliveryDate)
                    : null
                }
                onChange={(newValue) => {
                  formik.setFieldValue(
                    "estimatedDeliveryDate",
                    newValue ? newValue.toDate() : null
                  );
                }}
                slotProps={{
                  textField: {
                    name: "estimatedDeliveryDate",
                    error: Boolean(
                      formik.touched.estimatedDeliveryDate &&
                        formik.errors.estimatedDeliveryDate
                    ),
                    helperText:
                      formik.touched.estimatedDeliveryDate &&
                      formik.errors.estimatedDeliveryDate,
                  },
                }}
              />
            </LocalizationProvider>
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
                type="reset"
                variant="contained"
                onClick={() => formik.resetForm()}
                startIcon={<RefreshIcon />}
                fullWidth
              >
                Reset
              </Button>
              <Button variant="contained" type="submit" fullWidth>
                Submit
              </Button>
              <Button
                type="button"
                variant="contained"
                onClick={() => router.push("/dashboard")}
                startIcon={<ArrowBackIcon />}
                fullWidth
              >
                Back
              </Button>
            </Box>
          </Box>
        </form>
      </Box>
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
}
