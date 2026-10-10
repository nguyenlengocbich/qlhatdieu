import { NextRequest, NextResponse } from "next/server";
import { cookies } from "next/headers";
import { BACKEND_API_URL } from "@/lib/backend";

export async function POST(request: NextRequest) {
  try {
    const cookieStore = await cookies();
    const token = cookieStore.get("admin_session")?.value;
    const formData = await request.formData();

    const response = await fetch(`${BACKEND_API_URL}/api/upload`, {
      method: "POST",
      headers: token ? { Authorization: `Bearer ${token}` } : undefined,
      body: formData,
      cache: "no-store",
    });

    const data = await response.json();
    // Persist same-origin upload paths so images work through the Next.js rewrite.
    if (response.ok && typeof data?.url === "string") {
      try {
        const parsed = new URL(data.url);
        const backend = new URL(BACKEND_API_URL);
        if (parsed.origin === backend.origin && parsed.pathname.startsWith("/uploads/")) {
          data.url = `${parsed.pathname}${parsed.search}`;
        }
      } catch {
        // Keep the backend response unchanged if it is not an absolute URL.
      }
    }
    return NextResponse.json(data, { status: response.status });
  } catch (error) {
    console.error(error);
    return NextResponse.json({ error: "Không thể upload hình ảnh" }, { status: 500 });
  }
}
