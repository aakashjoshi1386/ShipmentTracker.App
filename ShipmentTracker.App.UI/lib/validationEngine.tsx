import * as Yup from "yup";

const origin = Yup.string().trim().required("Origin is required");
const destination = Yup.string().trim().required("Destination is required");
const shipDate = Yup.date()
  .nullable()
  .required("Ship date is required")
  .min(new Date().getDay() - 1, "Ship date must not be in the past.");

const eTA = Yup.date()
  .nullable()
  .required("ETA is required")
  .min(new Date(), "ETA date must not be in the past.")
  .when(
    "shipmentDate",
    ([shipmentDate]: [Date | null], schema: Yup.DateSchema<Date | null>) => {
      if (shipmentDate instanceof Date && !isNaN(shipmentDate.getTime())) {
        return schema.min(shipmentDate, "ETA must not be before Ship date.");
      }
      return schema;
    }
  )
  .typeError("ETA must be a valid date");

const carrierId = Yup.number().required("Carrier is required");

export const validationEngine = (validator: string) => {
  const validationSchema: Record<string, any> = {
    AddShipment: Yup.object({
      origin,
      destination,
      carrierId,
      shipmentDate: shipDate,
      estimatedDeliveryDate: eTA,
    }),
    EditShipment: Yup.object().shape({}),
  };

  return validationSchema[validator] || null;
};
