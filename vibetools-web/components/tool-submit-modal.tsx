'use client';

import {useState} from 'react';
import {useRouter} from 'next/navigation';
import {submitTool} from '@/lib/api';
import {Input} from '@/components/ui/input';
import {Textarea} from '@/components/ui/textarea';
import {Button} from '@/components/ui/button';
import {Label} from '@/components/ui/label';
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from '@/components/ui/dialog';
import {ApiErrorResponse, ToolCreateErrors} from "@/types";

export default function ToolSubmitModal() {
    const router = useRouter();
    const [open, setOpen] = useState(true);
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [submitted, setSubmitted] = useState(false);
    const [loading, setLoading] = useState(false);
    const [errors, setErrors] = useState<ToolCreateErrors>({});

    // If modal closed, navigate back
    const handleOpenChange = (isOpen: boolean) => {
        setOpen(isOpen);
        if (!isOpen) {
            router.back();
        }
    };

    const handleSubmit = async () => {
        const newErrors: typeof errors = {};

        if (!name.trim()) {
            newErrors.name = 'Name cannot be empty.';
        }

        if (!description.trim()) {
            newErrors.description = 'Description cannot be empty.';
        }

        if (Object.keys(newErrors).length > 0) {
            setErrors(newErrors);
            return;
        }

        try {
            setLoading(true);
            const tool = await submitTool({name, description});
            setErrors({});
            setSubmitted(true);

            router.push(`/tools/${tool.id}`);
        } catch (error: unknown) {
            const serverError = error as ApiErrorResponse<never>;
            if (serverError?.response?.status === 400 && serverError.response.data) {
                setErrors(serverError.response.data);
            } else {
                setErrors({server: 'Something went wrong. Please try again.'});
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <Dialog open={open} onOpenChange={handleOpenChange}>
            <DialogContent className="max-w-lg">
                <DialogHeader>
                    <DialogTitle>Submit a New Tool</DialogTitle>
                    <DialogDescription>
                        {submitted ? 'Tool submitted successfully!' : 'Enter details below.'}
                    </DialogDescription>
                </DialogHeader>

                {!submitted ? (
                    <form
                        className="space-y-4 my-4"
                        onSubmit={(e) => {
                            e.preventDefault();
                            handleSubmit();
                        }}>
                        <div>
                            <Label htmlFor="name" className="mb-2">
                                Tool Name
                            </Label>
                            <Input
                                id="name"
                                placeholder="Tool Name"
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                                disabled={loading}
                            />
                            {errors.name && <p className="text-red-600 text-sm mt-2">{errors.name}</p>}
                        </div>

                        <div>
                            <Label htmlFor="description" className="mb-2">
                                Description
                            </Label>
                            <Textarea
                                id="description"
                                placeholder="Tool Description"
                                value={description}
                                onChange={(e) => setDescription(e.target.value)}
                                disabled={loading}
                            />
                            {errors.description && <p className="text-red-600 text-sm mt-2">{errors.description}</p>}
                        </div>

                        {errors.server && <p className="text-red-600 text-sm mt-2">{errors.server}</p>}

                        <DialogFooter>
                            <Button
                                variant="outline"
                                onClick={() => setOpen(false)}
                                disabled={loading}>
                                Cancel
                            </Button>
                            <Button type="submit" disabled={loading}>
                                {loading ? 'Submitting...' : 'Submit'}
                            </Button>
                        </DialogFooter>
                    </form>
                ) : (
                    <DialogFooter>
                        <Button onClick={() => setOpen(false)}>Close</Button>
                    </DialogFooter>
                )}
            </DialogContent>
        </Dialog>
    );
}