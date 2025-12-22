import {
  Card,
  CardContent,
  Typography,
  Tooltip,
  CardActions,
} from "@mui/material";
import { DeskDto, DeskStatus } from "../types";
import ReserveDialog from "./ReserveDialog";
import CancelDialog from "./CancelDialog";
import { useState } from "react";
import DeskActionButton from "./DeskActionButton";
import { useTheme } from "@mui/material/styles";

export default function DeskCard({
  desk,
  userId,
  from,
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

  const theme = useTheme();

  const bgColor =
    desk.status === DeskStatus.Open
      ? theme.palette.success.light
      : desk.status === DeskStatus.Reserved
      ? theme.palette.error.light
      : theme.palette.warning.light;

  return (
    <Tooltip
      title={
        desk.status === DeskStatus.Reserved
          ? desk.reservedBy
          : desk.maintenanceMessage ?? ""
      }
    >
      <Card sx={{ backgroundColor: bgColor, height: 160, display: "flex", flexDirection: "column"  }}>
        <CardContent sx={{ flexGrow: 1 }}>
          <Typography variant="h6">Desk {desk.number}</Typography>
        </CardContent>

        <CardActions sx={{ minHeight: 48, px: 2, pb: 2 }}>
          {desk.status === DeskStatus.Open && (
            <DeskActionButton intent="reserve" onClick={() => setReserveOpen(true)}>Reserve</DeskActionButton>
          )}

          {desk.status === DeskStatus.Reserved && desk.isMine && (
            <DeskActionButton intent="cancel" onClick={() => setCancelOpen(true)}>Cancel</DeskActionButton>
          )}
        </CardActions>
        
        <ReserveDialog
          open={reserveOpen}
          onClose={() => setReserveOpen(false)}
          desk={desk}
          userId={userId}
          initialDate={from}
          onSuccess={onChanged}
        />

        <CancelDialog
          open={cancelOpen}
          onClose={() => setCancelOpen(false)}
          reservationId={desk.reservationId!}
          userId={userId}
          from={from}
          onSuccess={onChanged}
        />
      </Card>
    </Tooltip>
  );
}
