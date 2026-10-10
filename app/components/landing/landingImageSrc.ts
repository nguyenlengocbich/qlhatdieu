/** Convert .NET local upload URLs to the Next.js /uploads rewrite. */
export function landingImageSrc(value: string | null | undefined): string {
  const src = value?.trim() ?? "";
  if (!src) return "";
  try {
    const url = new URL(src);
    if ((url.hostname === "localhost" || url.hostname === "127.0.0.1") && url.port === "5000" && url.pathname.startsWith("/uploads/")) {
      return `${url.pathname}${url.search}`;
    }
  } catch { /* Relative URLs are already valid. */ }
  return src;
}
