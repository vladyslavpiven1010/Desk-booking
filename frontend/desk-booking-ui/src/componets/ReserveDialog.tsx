import { Dialog, Button, Typography, Stack } from "@mui/material";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { useState } from "react";
import dayjs, { Dayjs } from "dayjs";
import { Api } from "../api";

type Props = {
  open: boolean;
  onClose: () => void;
  desk: any;
  userId: string;
  initialDate: string;
  onSuccess: () => void;
};

export default function ReserveDialog({
  open,
  onClose,
  desk,
  userId,
  initialDate,
  onSuccess,
}: Props) {
  const [startDate, setStartDate] = useState<Dayjs | null>(
    dayjs(initialDate)
  );
  const [endDate, setEndDate] = useState<Dayjs | null>(
    dayjs(initialDate)
  );

  const canReserve =
    startDate &&
    endDate &&
    !endDate.isBefore(startDate);

  const reserve = async () => {
    if (!canReserve) return;

    await Api.createReservation({
      deskId: desk.deskId,
      userId,
      startDate: startDate!.format("YYYY-MM-DD"),
      endDate: endDate!.format("YYYY-MM-DD"),
    });

    onClose();
    onSuccess();
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <Stack padding={2} spacing={2}>
        <Typography>
          Reserve desk {desk.number}
        </Typography>

        <DatePicker
          label="Start date"
          value={startDate}
          onChange={setStartDate}
          slotProps={{ textField: { size: "small" } }}
        />

        <DatePicker
          label="End date"
          value={endDate}
          onChange={setEndDate}
          slotProps={{ textField: { size: "small" } }}
          minDate={startDate ?? undefined}
        />

        <Button
          onClick={reserve}
          disabled={!canReserve}
        >
          Confirm reservation
        </Button>

        <Button onClick={onClose}>
          Cancel
        </Button>
      </Stack>
    </Dialog>
  );
}
