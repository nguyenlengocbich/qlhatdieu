import { NextRequest, NextResponse } from "next/server";
import { cookies } from "next/headers";
import { BACKEND_API_URL } from "@/lib/backend";

async function proxy(id: string, method: string, body?: unknown) {
  const cookieStore = await cookies();
  const token = cookieStore.get("admin_session")?.value;
  return fetch(`${BACKEND_API_URL}/api/items/${id}`, {
    method,
    headers: {
      ...(body !== undefined ? { "Content-Type": "application/json" } : {}),
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    },
    ...(body !== undefined ? { body: JSON.stringify(body) } : {}),
    cache: "no-store",
  });
}

export async function PUT(request: NextRequest, { params }: { params: Promise<{ id: string }> }) {
  const { id } = await params;
  const body = await request.json();
  const response = await proxy(id, "PUT", body);
  const data = await response.json();
  return NextResponse.json(data, { status: response.status });
}

export async function DELETE(_request: NextRequest, { params }: { params: Promise<{ id: string }> }) {
  const { id } = await params;
  const response = await proxy(id, "DELETE");
  const data = await response.json();
  return NextResponse.json(data, { status: response.status });
}
