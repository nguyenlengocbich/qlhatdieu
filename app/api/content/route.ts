import { NextRequest, NextResponse } from "next/server";
import { BACKEND_API_URL } from "@/lib/backend";
import { cookies } from "next/headers";

export const dynamic = "force-dynamic";

export async function GET() {
  try {
    const response = await fetch(`${BACKEND_API_URL}/api/content`, { cache: "no-store" });
    const data = await response.json();
    return NextResponse.json(data, { status: response.status });
  } catch (error) {
    console.error(error);
    return NextResponse.json({ error: "Không thể lấy nội dung website" }, { status: 500 });
  }
}

export async function PUT(request: NextRequest) {
  try {
    const cookieStore = await cookies();
    const token = cookieStore.get("admin_session")?.value;
    const body = await request.json();

    const response = await fetch(`${BACKEND_API_URL}/api/content`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
      },
      body: JSON.stringify(body),
      cache: "no-store",
    });

    const data = await response.json();
    return NextResponse.json(data, { status: response.status });
  } catch (error) {
    console.error(error);
    return NextResponse.json({ error: "Không thể cập nhật nội dung website" }, { status: 500 });
  }
}
