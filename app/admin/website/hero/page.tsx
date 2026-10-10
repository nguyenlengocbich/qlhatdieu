import { redirect } from "next/navigation";
import { backendFetch } from "@/lib/backend";

export const dynamic = "force-dynamic";

export default async function HeroPage() {
  const response = await backendFetch("/api/sections");
  const sections = response.ok ? await response.json() : [];
  const section = sections.find((item: { key: string }) => item.key === "hero");
  if (section) redirect(`/admin/website/section/${section.id}`);
  redirect("/admin/website");
}
