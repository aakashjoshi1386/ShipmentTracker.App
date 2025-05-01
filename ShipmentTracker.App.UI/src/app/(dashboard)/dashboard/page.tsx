import { Typography } from "@mui/material";
import ShipmentGrid from "./components/ShipmentGrid";

export const metadata = {
  title: "Dashboard – Shipment Tracker",
};

export default function DashboardPage() {
  return (
    <>
      <Typography variant="h4" gutterBottom>
        Shipment Dashboard
      </Typography>
      <ShipmentGrid />
    </>
  );
}
