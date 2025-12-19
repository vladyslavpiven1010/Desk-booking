import { createTheme } from "@mui/material/styles";

const theme = createTheme({
  palette: {
    primary: {
      main: "#2F96FE",
      dark: "#196BCA",
      light: "#EAF4FF",
      contrastText: "#FFFFFF",
    },
    secondary: {
      main: "#14B8A6",
      dark: "#0D9488",
      light: "#E6FFFB",
      contrastText: "#FFFFFF",
    },
    success: {
      main: "#22C55E",
      light: "#E9FBEF",
    },
    warning: {
      main: "#F59E0B",
      light: "#FFF7E6",
    },
    error: {
      main: "#EF4444",
      light: "#FEECEC",
    },
    info: {
      main: "#3B82F6",
      light: "#EAF4FF",
    },
    background: {
      default: "#F6F8FB",
      paper: "#FFFFFF",
    },
    text: {
      primary: "#0F172A",
      secondary: "#475569",
    },
    divider: "#E2E8F0",
  },
  shape: {
    borderRadius: 12,
  },
  typography: {
    fontFamily:
      "Inter, system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Arial, sans-serif",
    button: {
      textTransform: "none",
      fontWeight: 600
    },
  },
  components: {
    MuiCssBaseline: {
      styleOverrides: {
        body: {
          backgroundColor: "#F6F8FB",
          color: "#0F172A",
        },
      },
    },
    MuiAppBar: {
      defaultProps: { elevation: 0 },
      styleOverrides: { root: { backgroundImage: "none" } },
    },
    MuiPaper: {
      defaultProps: { elevation: 0 },
      styleOverrides: { root: { backgroundImage: "none" } },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          boxShadow: "none",
          border: "1px solid #E2E8F0",
        },
      },
    },
    MuiTableContainer: {
      styleOverrides: {
        root: {
          boxShadow: "none",
          border: "1px solid #E2E8F0",
          borderRadius: 12,
          overflow: "hidden",
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: { borderRadius: 12 },
      },
    },
  },
});

export default theme;
