import { NextRequest, NextResponse } from "next/server";
import { cookies } from "next/headers";
import { BACKEND_API_URL } from "@/lib/backend";

export const dynamic = "force-dynamic";

async function callBackend(id: string, request: NextRequest, method: string, body?: unknown) {
  const cookieStore = await cookies();
  const token = cookieStore.get("admin_session")?.value;
  return fetch(`${BACKEND_API_URL}/api/sections/${id}`, {
    method,
    headers: {
      ...(body !== undefined ? { "Content-Type": "application/json" } : {}),
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    },
    ...(body !== undefined ? { body: JSON.stringify(body) } : {}),
    cache: "no-store",
  });
}

export async function GET(request: NextRequest, { params }: { params: Promise<{ id: string }> }) {
  const { id } = await params;
  try {
    const response = await callBackend(id, request, "GET");
    const data = await response.json();
    return NextResponse.json(data, { status: response.status });
  } catch (error) {
    console.error(error);
    return NextResponse.json({ error: "Không thể lấy section" }, { status: 500 });
  }
}

export async function PUT(request: NextRequest, { params }: { params: Promise<{ id: string }> }) {
  const { id } = await params;
  try {
    const body = await request.json();
    const response = await callBackend(id, request, "PUT", body);
    const data = await response.json();
    return NextResponse.json(data, { status: response.status });
  } catch (error) {
    console.error(error);
    return NextResponse.json({ error: "Không thể cập nhật section" }, { status: 500 });
  }
}
