import { useEffect, useState, useCallback } from "react";
import Grid from "@mui/material/Grid";
import { Api } from "../api";
import { DeskDto } from "../types";
import TopBar from "../componets/TopBar";
import DeskCard from "../componets/DeskCard";

export default function DesksPage() {
  const [userId, setUserId] = useState(
    localStorage.getItem("userId") ?? ""
  );

  const [date, setDate] = useState(
    localStorage.getItem("date") ?? "2025-01-10"
  );

  const [desks, setDesks] = useState<DeskDto[]>([]);

  const load = useCallback(async () => {
    if (!userId || !date) return;
    const data = await Api.getDesks(date, date, userId);
    setDesks(data);
  }, [date, userId]);

  useEffect(() => {
    load();
  }, [load]);

  return (
    <>
      <TopBar
        userId={userId}
        onUserChange={setUserId}
        date={date}
        onDateChange={setDate}
      />

      <Grid container spacing={2} padding={2}>
        {desks.map((desk) => (
          <Grid key={desk.deskId} size={{ xs: 12, sm: 6, md: 3 }}>
            <DeskCard
              desk={desk}
              userId={userId}
              from={date}
              to={date}
              onChanged={load}
            />
          </Grid>
        ))}
      </Grid>
    </>
  );
}
