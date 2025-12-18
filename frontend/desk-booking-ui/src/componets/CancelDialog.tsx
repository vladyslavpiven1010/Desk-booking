import { Dialog, Button, Stack } from "@mui/material";
import { Api } from "../api";
import { CancelMode } from "../types";

export default function CancelDialog({
  open,
  onClose,
  reservationId,
  userId,
  from,
  to,
  onSuccess,
}: any) {
  const cancelRange = async () => {
    await Api.cancelReservation(reservationId, {
      userId,
      mode: CancelMode.Range,
    });
    onClose();
    onSuccess();
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <Stack padding={2} spacing={1}>
        <Button onClick={cancelRange}>Cancel whole range</Button>
      </Stack>
    </Dialog>
  );
}
