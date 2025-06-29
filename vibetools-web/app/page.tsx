'use client';

import { useEffect, useState } from 'react';
import { fetchTools } from '@/lib/api';
import { Tool } from '@/types';
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import StarRating from '@/components/star-rating';
import { Spinner } from '@/components/ui/spinner';
import { Badge } from '@/components/ui/badge';
import { Input } from '@/components/ui/input';
import ToolSubmitModal from "@/components/tool-submit-modal";

export default function HomePage() {
    const [tools, setTools] = useState<Tool[]>([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [showSubmitModal, setShowSubmitModal] = useState(false);

    // Debounced search
    useEffect(() => {
        const query = searchTerm.trim();
        if (query && query.length <= 2) return;

        const handler = setTimeout(async () => {
            setLoading(true);
            const tools = await fetchTools(query);
            setTools(tools);
            setLoading(false);
        }, 300);
        return () => clearTimeout(handler);
    }, [searchTerm]);

    return (
        <div className="max-w-3xl mx-auto p-4 space-y-4">
            <div className="flex justify-between items-center">
                <h1 className="text-2xl font-bold">VibeTools</h1>
                <Button onClick={() => setShowSubmitModal(true)}>Submit a new tool</Button>
            </div>

            <div className="flex space-x-2">
                <Input
                    placeholder="Search tools..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="flex-1"
                />
            </div>

            <div className="space-y-6">
                {loading ? (
                    <div className="flex justify-center items-center h-48">
                        <Spinner />
                    </div>
                ) : (
                    tools.map((tool) => (
                        <a key={tool.id} href={`/tools/${tool.id}`}>
                            <Card className="space-y-8 mt-6">
                                <CardContent className="p-4 space-y-4">
                                    <div className="space-y-2">
                                        <div className="flex space-x-4">
                                            <div className="text-xl font-semibold">{tool.name}</div>
                                            {tool.communityFavourite && (
                                                <Badge className="bg-green-500 text-white">
                                                    Community Favorite
                                                </Badge>
                                            )}
                                        </div>
                                        <div>{tool.description}</div>
                                    </div>
                                    <div className="flex space-x-2 items-center">
                                        <StarRating value={tool.avgRating} readOnly />
                                        <p>({tool.reviewCount} reviews)</p>
                                    </div>
                                </CardContent>
                            </Card>
                        </a>
                    ))
                )}
            </div>

            {showSubmitModal && (
                <ToolSubmitModal/>
            )}
        </div>
    );
}
