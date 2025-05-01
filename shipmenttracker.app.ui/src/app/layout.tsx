"use client";

import * as React from "react";
import { ThemeProvider, CssBaseline, Container } from "@mui/material";
import { NextAppProvider } from "@toolpad/core/nextjs";
import theme from "../themes/theme";
import createEmotionCache from "../utils/createEmotionCache";
import { CacheProvider } from "@emotion/react";

const clientSideEmotionCache = createEmotionCache();

export default function Layout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>
        <CacheProvider value={clientSideEmotionCache}>
          <ThemeProvider theme={theme}>
            <CssBaseline />
            <NextAppProvider>
              <Container maxWidth={"xl"}>{children}</Container>
            </NextAppProvider>
          </ThemeProvider>
        </CacheProvider>
      </body>
    </html>
  );
}
