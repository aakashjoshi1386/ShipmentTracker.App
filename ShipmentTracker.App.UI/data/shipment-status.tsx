import { ShipmentStatus } from "../types";

export const getShipmentStatus = (): ShipmentStatus[] => {
  return [
    { id: 1, name: "Processing" },
    { id: 2, name: "Shipped" },
    { id: 3, name: "In Transit" },
    { id: 4, name: "Out For Delivery" },
    { id: 5, name: "Delivered" },
    { id: 6, name: "Picked Up" },
    { id: 7, name: "Cancelled" },
  ];
};
