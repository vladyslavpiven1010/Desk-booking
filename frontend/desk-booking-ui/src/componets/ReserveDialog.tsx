import { Dialog, Button, Typography } from "@mui/material";
import { Api } from "../api";

export default function ReserveDialog({
  open,
  onClose,
  desk,
  userId,
  from,
  to,
  onSuccess,
}: any) {
  const reserve = async () => {
    await Api.createReservation({
      deskId: desk.deskId,
      userId,
      startDate: from,
      endDate: to,
    });
    onClose();
    onSuccess();
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <Typography padding={2}>
        Reserve desk {desk.number} from {from} to {to}?
      </Typography>
      <Button onClick={reserve}>Confirm</Button>
    </Dialog>
  );
}
