import type { NextConfig } from "next";

const backendUrl = process.env.BACKEND_API_URL || "http://localhost:5000";

const nextConfig: NextConfig = {
  async rewrites() {
    return [{ source: "/uploads/:path*", destination: `${backendUrl}/uploads/:path*` }];
  },
};

export default nextConfig;
