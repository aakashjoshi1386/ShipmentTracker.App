"use client";
import * as React from "react";
import { DataGrid, GridColDef, GridPaginationModel } from "@mui/x-data-grid";
import {
  Box,
  MenuItem,
  FormControl,
  InputLabel,
  Select,
  SelectChangeEvent,
  Button,
} from "@mui/material";
import api from "../../../../../lib/api";
import {
  Carrier,
  GetShipmentsRequest,
  Shipment,
  ShipmentStatus,
} from "../../../../..//types";
import RefreshIcon from "@mui/icons-material/Refresh";
import { getShipmentStatus } from "../../../../../data/shipment-status";
import EditShipmentModal from "@/app/edit-shipment/edit-shipment-modal";
import EditIcon from "@mui/icons-material/Edit";
import IconButton from "@mui/material/IconButton";

export default function ShipmentGrid() {
  const [open, setEditShipmentOpen] = React.useState(false);
  const [id, setId] = React.useState<number>(0);
  const [status, setStatus] = React.useState(0);
  const [shipments, setShipments] = React.useState<Shipment[]>([]);
  const [pageSize, setPageSize] = React.useState(10);
  const [page, setPage] = React.useState(1);
  const [totalCount, setTotalCount] = React.useState(0);
  const shipmentStatus = React.useMemo<ShipmentStatus[]>(
    () => getShipmentStatus(),
    []
  );
  const [refreshGrid, setRefreshGrid] = React.useState(false);
  const [carriers, setCarriers] = React.useState<Carrier[]>([]);
  const [statusFilter, setStatusFilter] = React.useState(null);
  const [carrierFilter, setCarrierFilter] = React.useState(null);

  React.useEffect(() => {
    api.get("/carriers").then((res) => setCarriers(res.data));
  }, []);

  React.useEffect(() => {
    const params: GetShipmentsRequest = {
      page,
      pageSize,
    };
    if (statusFilter) params.statusId = statusFilter;
    if (carrierFilter) params.carrierId = carrierFilter;

    api
      .get("/shipments", { params })
      .then((res) => {
        setShipments(res.data.items);
        setTotalCount(res.data.totalCount);
      })
      .catch((error) => {
        console.error("Error fetching shipments:", error);
        setShipments([]);
        setTotalCount(0);
      });
  }, [page, pageSize, statusFilter, carrierFilter, refreshGrid]);

  const handleStatusChange = async (id: number, statusId: number) => {
    setEditShipmentOpen(true);
    setId(id);
    setStatus(statusId);
  };

  const columns: GridColDef[] = [
    { field: "origin", headerName: "Origin", flex: 1 },
    { field: "destination", headerName: "Destination", flex: 1 },
    {
      field: "carrier",
      headerName: "Carrier",
      flex: 1,
    },
    { field: "shipmentDate", headerName: "Shipment Date", flex: 1 },
    { field: "estimatedDeliveryDate", headerName: "ETA", flex: 1 },
    {
      field: "statusId",
      headerName: "Status",
      flex: 1,
      valueGetter: (params) => {
        const statusObj = shipmentStatus.find((s) => s.id === params);
        return statusObj ? statusObj.name : "Unknown";
      },
    },
    {
      field: "actions",
      headerName: "Actions",
      flex: 0.5,
      sortable: false,
      filterable: false,
      renderCell: (params) => (
        <IconButton
          color="primary"
          onClick={() => handleStatusChange(params.row.id, params.row.statusId)}
        >
          <EditIcon />
        </IconButton>
      ),
    },
  ];

  const reset = () => {
    setPage(1);
    setPageSize(10);
    setStatusFilter(null);
    setCarrierFilter(null);
  };

  const handleRefresh = () => {
    setRefreshGrid((prev) => !prev);
    setId(0);
    setStatus(0);
  };
  return (
    <>
      <Box>
      <Box sx={{ display: "flex", flexWrap: "wrap", gap: 1, mb: 2 }}>
        <FormControl size="small" sx={{ m: 1, minWidth: { xs: "100%", sm: 120 } }}>
            <InputLabel>Status</InputLabel>
            <Select
              value={statusFilter}
              onChange={(e: SelectChangeEvent) =>
                setStatusFilter(e.target.value)
              }
              label="Status"
              autoWidth
            >
              {shipmentStatus.map((status) => (
                <MenuItem key={status.id} value={status.id}>
                  {status.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>

          <FormControl size="small" sx={{ m: 1, minWidth: { xs: "100%", sm: 120 } }}>
            <InputLabel>Carrier</InputLabel>
            <Select
              value={carrierFilter}
              onChange={(e: SelectChangeEvent) =>
                setCarrierFilter(e.target.value)
              }
              label="Carrier"
              autoWidth
            >
              {carriers.map((carrier) => (
                <MenuItem key={carrier.id} value={carrier.id}>
                  {carrier.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <FormControl size="small" sx={{ m: 1, minWidth: { xs: "100%", sm: 120 } }}>
            <Button
              variant="contained"
              endIcon={<RefreshIcon />}
              onClick={reset}
            >
              Refresh
            </Button>
          </FormControl>
        </Box>
        <Box sx={{ height: "auto", width: "100%", overflowX: "auto", overflowY: "hidden" }}>
          <DataGrid
            rows={shipments}
            columns={columns}
            getRowId={(row) => row.id}
            paginationModel={{ page: page - 1, pageSize }}
            onPaginationModelChange={(model: GridPaginationModel) => {
              setPage(model.page + 1);
              setPageSize(model.pageSize);
            }}
            pageSizeOptions={[5, 10, 20]}
            rowCount={totalCount}
            paginationMode="server"
            disableColumnMenu
            disableColumnFilter
            disableColumnSelector
            disableColumnSorting
            autoHeight
          />
        </Box>
      </Box>
      <EditShipmentModal
        open={open}
        onClose={() => setEditShipmentOpen(false)}
        onSuccess={() => {
          handleRefresh();
        }}
        id={id}
        statusId={status}
      />
    </>
  );
}
