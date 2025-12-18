import { useEffect, useState } from "react";
import {
  AppBar,
  Toolbar,
  Select,
  MenuItem,
  CircularProgress,
  Box,
  Button,
} from "@mui/material";
import { useNavigate, useLocation } from "react-router-dom";
import { Api } from "../api";
import { UserDto } from "../types";

type Props = {
  userId: string;
  onUserChange: (id: string) => void;
};

export default function TopBar({ userId, onUserChange }: Props) {
  const [users, setUsers] = useState<UserDto[]>([]);
  const [loading, setLoading] = useState(true);

  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    Api.getUsers()
      .then(setUsers)
      .finally(() => setLoading(false));
  }, []);

  return (
    <AppBar position="static">
      <Toolbar sx={{ gap: 2 }}>
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
