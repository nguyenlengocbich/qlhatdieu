import { NextResponse } from "next/server";
import { BACKEND_API_URL } from "@/lib/backend";
import { cookies } from "next/headers";

export const dynamic = "force-dynamic";

export async function GET() {
  try {
    const cookieStore = await cookies();
    const token = cookieStore.get("admin_session")?.value;
    const response = await fetch(`${BACKEND_API_URL}/api/sections`, {
      headers: token ? { Authorization: `Bearer ${token}` } : undefined,
      cache: "no-store",
    });
    const data = await response.json();
    return NextResponse.json(data, { status: response.status });
  } catch (error) {
    console.error(error);
    return NextResponse.json({ error: "Không thể lấy dữ liệu section" }, { status: 500 });
  }
}
