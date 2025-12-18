import { useEffect, useState } from "react";
import { Api } from "../api";
import TopBar from "../componets/TopBar";

export default function ProfilePage() {
  const [userId, setUserId] = useState(
    localStorage.getItem("userId") ??
      "11111111-1111-1111-1111-111111111111"
  );

  const [profile, setProfile] = useState<any>(null);

  useEffect(() => {
    Api.getProfile(userId).then(setProfile);
  }, [userId]);

  if (!profile) return null;

  return (
    <>
      <TopBar userId={userId} onUserChange={setUserId} />

      <h2>{profile.fullName}</h2>

      <h3>Current reservations</h3>
      {profile.currentReservations.map((r: any) => (
        <div key={r.id}>
          Desk {r.deskNumber}: {r.startDate} → {r.endDate}
        </div>
      ))}

      <h3>Past reservations</h3>
      {profile.pastReservations.map((r: any) => (
        <div key={r.id}>
          Desk {r.deskNumber}: {r.startDate} → {r.endDate}
        </div>
      ))}
    </>
  );
}
