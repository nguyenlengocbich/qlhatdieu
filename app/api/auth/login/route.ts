import { NextRequest, NextResponse } from "next/server";
import { BACKEND_API_URL } from "@/lib/backend";

export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    const response = await fetch(`${BACKEND_API_URL}/api/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body),
      cache: "no-store",
    });

    const data = await response.json();
    if (!response.ok) return NextResponse.json(data, { status: response.status });

    const result = NextResponse.json(data);
    result.cookies.set("admin_session", data.token, {
      httpOnly: true,
      secure: process.env.NODE_ENV === "production",
      sameSite: "lax",
      path: "/",
      expires: new Date(data.expiresAt),
    });
    return result;
  } catch (error) {
    console.error("Login proxy error:", error);
    return NextResponse.json(
      { message: "Không thể kết nối đến backend .NET" },
      { status: 500 }
    );
  }
}
