import { Button, ButtonProps, useTheme } from "@mui/material";

type Intent = "reserve" | "cancel";

type Props = ButtonProps & {
  intent: Intent;
};

export default function DeskActionButton({ intent, sx, ...props }: Props) {
  const theme = useTheme();

  const cfg = intent === "reserve"
    ? {
        text: theme.palette.success.main,
        hoverBg: theme.palette.action.hover,
      }
    : {
        text: theme.palette.error.main,
        hoverBg: theme.palette.action.hover,
      };

  return (
    <Button
      {...props}
      sx={[{ transition: "all 0.15s ease", color: cfg.text, border: "1px solid rgba(0,0,0,0.12)",
          "&:hover": {
            backgroundColor: cfg.hoverBg,
            color: cfg.text,
          },
        },
        ...(Array.isArray(sx) ? sx : sx ? [sx] : []),
      ]}
    />
  );
}
