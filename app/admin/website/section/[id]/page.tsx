import { notFound } from "next/navigation";
import { backendFetch } from "@/lib/backend";
import SectionEditor from "./SectionEditor";

export const dynamic = "force-dynamic";

export default async function SectionEditPage({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = await params;

  const sectionResponse = await backendFetch(`/api/sections/${Number(id)}`);
  const section = sectionResponse.ok ? await sectionResponse.json() : null;

  if (!section) {
    notFound();
  }

  const itemsResponse = await backendFetch(`/api/sections/${Number(id)}/items`);
  const items = itemsResponse.ok ? await itemsResponse.json() : [];

  return (
    <SectionEditor
      section={{
        id: section.id,
        key: section.key,
        name: section.name,
        title: section.title ?? "",
        subtitle: section.subtitle ?? "",
        description: section.description ?? "",
        content: section.content ?? "",
        image: section.image ?? "",
        status: section.status,
      }}
      items={items}
    />
  );
}