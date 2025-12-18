import { Routes, Route } from "react-router-dom";
import DesksPage from "./pages/DesksPage";
import ProfilePage from "./pages/ProfilePage";

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<DesksPage />} />
      <Route path="/profile" element={<ProfilePage />} />
    </Routes>
  );
}

