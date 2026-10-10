import { publicBackendFetch } from "./backend";
export type SectionItem = { id:number; sectionKey:string; title:string; description:string|null; icon:string|null; image:string|null; sortOrder:number; status:string };
export type SectionContent = { id:number; key:string; name:string; title:string|null; subtitle:string|null; description:string|null; content:string|null; image:string|null; status:string; updatedAt:string; items:SectionItem[] };
export type HeroContent = { badge:string; title:string; description:string; primaryButton:string; secondaryButton:string };
export type CTAContent = { badge:string|null; title:string|null; description:string|null; status:string; updatedAt:string };
export type SiteContent = { hero:HeroContent; sections:SectionContent[]; cta:CTAContent };
export async function getSiteContent(): Promise<SiteContent> {
  const response = await publicBackendFetch("/api/content");
  if (!response.ok) throw new Error("Không thể lấy nội dung từ backend .NET");
  return response.json();
}
