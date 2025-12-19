import {
  Card,
  CardContent,
  Typography,
  Button,
  Tooltip,
} from "@mui/material";
import { DeskDto, DeskStatus } from "../types";
import ReserveDialog from "./ReserveDialog";
import CancelDialog from "./CancelDialog";
import { useState } from "react";

type Props = {
  desk: DeskDto;
  userId: string;
  from: string;
  to: string;
  onChanged: () => void;
};

export default function DeskCard({
  desk,
  userId,
  from,
  to,
  onChanged,
}: {
  desk: DeskDto;
  userId: string;
  from: string;
  to: string;
  onChanged: () => void;
}) {
  const [reserveOpen, setReserveOpen] = useState(false);
  const [cancelOpen, setCancelOpen] = useState(false);

  const color =
    desk.status === DeskStatus.Open
      ? "#c8e6c9"
      : desk.status === DeskStatus.Reserved
      ? "#ffcdd2"
      : "#fff9c4";

  return (
    <Tooltip
      title={
        desk.status === DeskStatus.Reserved
          ? desk.reservedBy
          : desk.maintenanceMessage ?? ""
      }
    >
      <Card sx={{ backgroundColor: color }}>
        <CardContent>
          <Typography variant="h6">Desk {desk.number}</Typography>

          {desk.status === DeskStatus.Open && (
            <Button onClick={() => setReserveOpen(true)}>Reserve</Button>
          )}

          {desk.status === DeskStatus.Reserved && desk.isMine && (
            <Button onClick={() => setCancelOpen(true)}>Cancel</Button>
          )}
        </CardContent>

        <ReserveDialog
          open={reserveOpen}
          onClose={() => setReserveOpen(false)}
          desk={desk}
          userId={userId}
          from={from}
          to={to}
          onSuccess={onChanged}
        />

        <CancelDialog
          open={cancelOpen}
          onClose={() => setCancelOpen(false)}
          reservationId={desk.reservationId!}
          userId={userId}
          from={from}
          to={to}
          onSuccess={onChanged}
        />
      </Card>
    </Tooltip>
  );
}
