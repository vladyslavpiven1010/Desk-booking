import { useEffect, useState } from "react";
import {
  AppBar,
  Toolbar,
  Select,
  MenuItem,
  CircularProgress,
  Box,
  Button
} from "@mui/material";
import { useNavigate, useLocation } from "react-router-dom";
import dayjs, { Dayjs } from "dayjs";
import { Api } from "../api";
import { UserDto } from "../types";
import { DatePicker } from "@mui/x-date-pickers";

type Props = {
  userId: string;
  onUserChange: (id: string) => void;

  date: string;
  onDateChange: (date: string) => void;
};

export default function TopBar({ 
  userId,
  onUserChange,
  date,
  onDateChange, 
}: Props) {
  const [users, setUsers] = useState<UserDto[]>([]);
  const [loading, setLoading] = useState(true);

  const navigate = useNavigate();
  const location = useLocation();

  const [selectedDate, setSelectedDate] = useState<Dayjs | null>(
    dayjs(date)
  );

  useEffect(() => {
    Api.getUsers()
      .then(setUsers)
      .finally(() => setLoading(false));
  }, []);

  const handleDateChange = (value: Dayjs | null) => {
    setSelectedDate(value);
    if (!value) return;

    const d = value.format("YYYY-MM-DD");
    localStorage.setItem("date", d);
    onDateChange(d);
  };

  return (
    <AppBar position="static">
      <Toolbar sx={{ gap: 2, flexWrap: "wrap" }}>
        {/* User selector */}
        {loading ? (
          <CircularProgress size={24} color="inherit" />
        ) : (
          <Box sx={{ minWidth: 220 }}>
            <Select
              value={userId}
              onChange={(e) => {
                const id = e.target.value;
                localStorage.setItem("userId", id);
                onUserChange(id);
              }}
              sx={{ color: "white" }}
            >
              {users.map((u) => (
                <MenuItem key={u.id} value={u.id}>
                  {u.fullName}
                </MenuItem>
              ))}
            </Select>
          </Box>
        )}

        {/* Date range picker */}
        <DatePicker
          label="Date"
          value={selectedDate}
          onChange={handleDateChange}
          slotProps={{ textField: { size: "small" } }}
        />

        {/* Navigation */}
        <Button
          color="inherit"
          onClick={() => navigate("/")}
          disabled={location.pathname === "/"}
        >
          Desks
        </Button>

        <Button
          color="inherit"
          onClick={() => navigate("/profile")}
          disabled={location.pathname === "/profile"}
        >
          Profile
        </Button>
      </Toolbar>
    </AppBar>
  );
}
