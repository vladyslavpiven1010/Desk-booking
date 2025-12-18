export enum DeskStatus {
  Open = 0,
  Reserved = 1,
  Maintenance = 2,
}

export type DeskDto = {
  deskId: string;
  number: number;
  status: DeskStatus;
  reservedBy?: string;
  reservedByUserId?: string;
  maintenanceMessage?: string;
  isMine: boolean;
  reservationId?: string;
};

export type CreateReservationDto = {
  deskId: string;
  userId: string;
  startDate: string;
  endDate: string;
};

export enum CancelMode {
  Day = 0,
  Range = 1,
}

export type CancelReservationDto = {
  userId: string;
  mode: CancelMode;
  day?: string;
};

export type ReservationDto = {
  id: string;
  deskId: string;
  deskNumber: number;
  userId: string;
  userFullName: string;
  startDate: string;
  endDate: string;
  isCanceled: boolean;
  canceledAt?: string;
};

export type UserDto = {
  id: string;
  fullName: string;
};

export type ProfileDto = {
  userId: string;
  fullName: string;
  currentReservations: ReservationDto[];
  pastReservations: ReservationDto[];
};
