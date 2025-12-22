import { Dialog, Button, Stack, Typography } from "@mui/material";
import { Api } from "../api";
import { CancelMode } from "../types";

type Props = {
  open: boolean;
  onClose: () => void;
  reservationId: string;
  userId: string;
  from: string; 
  onSuccess: () => void;
};

export default function CancelDialog({
  open,
  onClose,
  reservationId,
  userId,
  from,
  onSuccess,
}: Props) {
  const cancelDay = async () => {
    await Api.cancelReservation(reservationId, {
      userId,
      mode: CancelMode.Day,
      day: from,
    });
    onClose();
    onSuccess();
  };

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
      <Stack padding={2} spacing={2}>
        <Typography>
          How do you want to cancel this reservation?
        </Typography>

        <Button
          color="warning"
          onClick={cancelDay}
        >
          Cancel only this day
        </Button>

        <Button
          color="error"
          onClick={cancelRange}
        >
          Cancel whole reservation
        </Button>

        <Button onClick={onClose}>
          Close
        </Button>
      </Stack>
    </Dialog>
  );
}
