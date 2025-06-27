import {Tool, Review} from '@/types';

const apiBase = process.env.NEXT_PUBLIC_API_BASE;

export async function fetchTools(searchTerm: string = ""): Promise<Tool[]> {
    let url = `${apiBase}/tools`;

    if (searchTerm.trim() !== "") {
        url = `${url}?query=${searchTerm}`;
    }

    const res = await fetch(url);
    return res.json();
}

export async function fetchTool(id: number): Promise<Tool> {
    const res = await fetch(`${apiBase}/tools/${id}`);
    return res.json();
}

export async function submitTool(data: { name: string; description: string }): Promise<Tool> {
    const res = await fetch(`${apiBase}/tools`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(data),
    });
    return res.json();
}

export async function submitReview(toolId: number, data: { rating: number; comment: string }): Promise<Review> {
    const res = await fetch(`${apiBase}/tools/${toolId}/reviews`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(data),
    });
    return res.json();
}
