import axios from "axios";
import {
  DeskDto,
  CreateReservationDto,
  CancelReservationDto,
  ProfileDto,
  UserDto,
} from "./types";

const API_BASE_URL = "http://localhost:5063";

const api = axios.create({
  baseURL: `${API_BASE_URL}/api`,
});

export const Api = {
  // USERS
  getUsers: async (): Promise<UserDto[]> => {
    const res = await api.get<UserDto[]>("/users");
    return res.data;
  },

  // DESKS
  getDesks: async (
    from: string,
    to: string,
    currentUserId: string
  ): Promise<DeskDto[]> => {
    const res = await api.get<DeskDto[]>("/desks", {
      params: { from, to, currentUserId },
    });
    return res.data;
  },

  // RESERVATIONS
  createReservation: async (dto: CreateReservationDto) => {
    const res = await api.post("/reservations", dto);
    return res.data;
  },

  cancelReservation: async (
    reservationId: string,
    dto: CancelReservationDto
  ) => {
    await api.post(`/reservations/${reservationId}/cancel`, dto);
  },

  // PROFILE
  getProfile: async (userId: string): Promise<ProfileDto> => {
    const res = await api.get<ProfileDto>("/profile", {
      params: { userId },
    });
    return res.data;
  },
};
