'use client';

import {useState} from 'react';
import {Star} from 'lucide-react';
import {cn} from '@/lib/utils';

interface StarRatingProps {
    value: number;
    onChange?: (value: number) => void;
    max?: number;
    readOnly?: boolean;
}

export default function StarRating({
                                       value,
                                       onChange,
                                       max = 5,
                                       readOnly = false,
                                   }: StarRatingProps) {
    const [hovered, setHovered] = useState<number | null>(null);

    return (
        <div className="flex gap-1">
            {Array.from({length: max}, (_, i) => i + 1).map((star) => (
                <Star
                    key={star}
                    size={20}
                    className={cn(
                        'cursor-pointer transition-colors',
                        (hovered ?? value) >= star ? 'fill-yellow-400 text-yellow-400' : 'text-gray-300',
                        readOnly && 'cursor-default'
                    )}
                    onClick={() => !readOnly && onChange?.(star)}
                    onMouseEnter={() => !readOnly && setHovered(star)}
                    onMouseLeave={() => !readOnly && setHovered(null)}
                />
            ))}
        </div>
    );
}
