import { cookies } from "next/headers";
export const BACKEND_API_URL = process.env.BACKEND_API_URL || "http://localhost:5000";
export async function backendFetch(path: string, init: RequestInit = {}) {
  const cookieStore = await cookies();
  const token = cookieStore.get("admin_session")?.value;
  const headers = new Headers(init.headers);
  headers.set("Accept", "application/json");
  if (token) headers.set("Authorization", `Bearer ${token}`);
  return fetch(`${BACKEND_API_URL}${path}`, { ...init, headers, cache: "no-store" });
}
export async function publicBackendFetch(path: string, init: RequestInit = {}) {
  const headers = new Headers(init.headers);
  headers.set("Accept", "application/json");
  return fetch(`${BACKEND_API_URL}${path}`, { ...init, headers, cache: "no-store" });
}
