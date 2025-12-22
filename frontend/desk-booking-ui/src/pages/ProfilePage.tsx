import { useEffect, useState } from "react";
import {
  Box,
  ToggleButton,
  ToggleButtonGroup,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Typography,
  CircularProgress,
} from "@mui/material";
import { Api } from "../api";
import TopBar from "../componets/TopBar";
import dayjs from "dayjs";

type ViewMode = "current" | "past";

export default function ProfilePage() {
  const [userId, setUserId] = useState(
    localStorage.getItem("userId") ??
      "11111111-1111-1111-1111-111111111111"
  );

  const [date, setDate] = useState(
    localStorage.getItem("date") ?? "2025-01-10"
  );

  const [profile, setProfile] = useState<any>(null);
  const [view, setView] = useState<ViewMode>("current");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!userId) return;

    setLoading(true);
    Api.getProfile(userId)
      .then(setProfile)
      .finally(() => setLoading(false));
  }, [userId]);

  const rows =
    view === "current"
      ? profile?.currentReservations ?? []
      : profile?.pastReservations ?? [];

  return (
    <>
      <TopBar
        userId={userId}
        onUserChange={setUserId}
        date={date}
        onDateChange={setDate}
      />

      <Box sx={{ p: 3 }}>
        <Typography variant="h5" gutterBottom>
          {profile?.fullName ?? "Profile"}
        </Typography>

        {/* Switcher */}
        <ToggleButtonGroup
          value={view}
          exclusive
          onChange={(_, v) => v && setView(v)}
          sx={{ mb: 2 }}
        >
          <ToggleButton value="current">
            Current reservations
          </ToggleButton>
          <ToggleButton value="past">
            Past reservations
          </ToggleButton>
        </ToggleButtonGroup>

        {/* Content */}
        {loading ? (
          <Box sx={{ display: "flex", justifyContent: "center", mt: 4 }}>
            <CircularProgress />
          </Box>
        ) : (
          <TableContainer component={Paper} elevation={0} variant="outlined" sx={{ boxShadow: "none" }}>
            <Table size="small" sx={{ tableLayout: "fixed" }}>
              <TableHead>
                <TableRow>
                  <TableCell sx={{
                    whiteSpace: "nowrap",
                    overflow: "hidden",
                    textOverflow: "ellipsis",
                  }}><b>Desk</b></TableCell>
                  <TableCell sx={{
                    whiteSpace: "nowrap",
                    overflow: "hidden",
                    textOverflow: "ellipsis",
                  }}><b>Start time</b></TableCell>
                  <TableCell sx={{
                    whiteSpace: "nowrap",
                    overflow: "hidden",
                    textOverflow: "ellipsis",
                  }}><b>Finish time</b></TableCell>
                </TableRow>
              </TableHead>

              <TableBody>
                {rows.length === 0 ? (
                  <TableRow>
                    <TableCell colSpan={3} align="center">
                      No reservations
                    </TableCell>
                  </TableRow>
                ) : (
                  rows.map((r: any) => (
                    <TableRow key={r.id}>
                      <TableCell>
                        Desk {r.deskNumber}
                      </TableCell>
                      <TableCell>
                        {dayjs(r.startDate).format("DD/MM/YYYY")}
                      </TableCell>
                      <TableCell>
                        {dayjs(r.endDate).format("DD/MM/YYYY")}
                      </TableCell>
                    </TableRow>
                  ))
                )}
              </TableBody>
            </Table>
          </TableContainer>
        )}
      </Box>
    </>
  );
}
