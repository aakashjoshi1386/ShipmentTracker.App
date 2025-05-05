export interface Shipment {
  id: number;
  origin: string;
  destination: string;
  carrierId: number;
  shipmentDate: string;
  estimatedDeliveryDate: string;
  statusId: number;
}

export interface Carrier {
  id: number;
  name: string;
}

export interface ShipmentStatus {
  id: number;
  name: string;
}

export interface GetShipmentsRequest {
  page: number;
  pageSize: number;
  statusId?: number;
  carrierId?: number;
}

export interface AddShipmentsRequest {
  origin: string;
  destination: string;
  carrierId: number | null;
  shipmentDate: Date | null;
  estimatedDeliveryDate: Date | null;
}
