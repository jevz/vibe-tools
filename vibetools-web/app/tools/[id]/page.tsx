'use client';

import {useEffect, useState} from 'react';
import {fetchTool, submitReview} from '@/lib/api';
import {useParams, useRouter} from 'next/navigation';
import {Tool, Review} from '@/types';
import StarRating from "@/components/star-rating";
import {Label} from "@/components/ui/label";
import {Button} from "@/components/ui/button";
import {Textarea} from "@/components/ui/textarea";
import {Spinner} from '@/components/ui/spinner';

export default function ToolDetailsPage() {
    const {id} = useParams();
    const router = useRouter();
    const toolId = Number(id);
    const [tool, setTool] = useState<Tool | null>(null);
    const [comment, setComment] = useState('');
    const [rating, setRating] = useState(1);
    const [submitted, setSubmitted] = useState(false);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchTool(toolId).then((data) => {
            setTool(data);
            setLoading(false);
        });
    }, [toolId]);

    const handleSubmit = async () => {
        if (comment.trim() === '') {
            alert('Please enter a comment.');
            return;
        }

        await submitReview(toolId, {rating, comment});
        fetchTool(toolId).then(setTool);
        setComment('');
        setRating(0);
        setSubmitted(true);
    };

    if (loading) return (
        <div className="flex justify-center items-center h-48">
            <Spinner/>
        </div>
    );
    if (!tool) return <div>Tool not found.</div>;

    return (
        <div className="max-w-2xl mx-auto p-4">
            <div className="flex items-center justify-between">
                <h1 className="text-2xl font-bold">{tool.name}</h1>
                <Button onClick={() => router.back()}>
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5"
                         stroke="currentColor" className="size-5">
                        <path d="m11.25 9-3 3m0 0 3 3m-3-3h7.5M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z"/>
                    </svg>
                    <div>Back to List</div>
                </Button>
            </div>
            <p>{tool.description}</p>

            <div className="mt-6 w-full">
                <div className="font-semibold">Add a Review</div>
                <div className="space-y-4 mt-4">
                    <StarRating value={rating} onChange={setRating} readOnly={submitted}/>
                    <div>
                        <Label className="mb-2" htmlFor="comment">Your thoughts...</Label>
                        <Textarea
                            id="comment"
                            required={true}
                            disabled={submitted}
                            value={comment}
                            onChange={(e) => setComment(e.target.value)}
                            placeholder="Your thoughts..."
                        />
                    </div>
                    <div className="flex justify-end w-full">
                        <Button onClick={handleSubmit} disabled={submitted}>
                            Submit Review
                        </Button>
                    </div>
                    {submitted && <p className="text-green-600 mt-4">Review submitted successfully!</p>}
                </div>
            </div>

            <h2 className="text-xl mt-6 font-semibold">Reviews</h2>
            {tool.reviews?.map((r: Review) => (
                <div key={r.id} className="border-b py-2">
                    <p>{r.comment}</p>
                    <div className="pt-4">
                        <StarRating value={r.rating} readOnly={true}/>
                    </div>
                </div>
            ))}

            {tool.reviews?.length === 0 && (
                <div className="py-2">
                    No reviews yet. Be the first to add one!
                </div>
            )}
        </div>
    );
}
