import { ChatMessageDto, FriendDto } from '@/shared/service-proxies/service-proxies';

export class ChatFriendDto extends FriendDto {
    public messages: ChatMessageDto[];
    public allPreviousMessagesLoaded = false;
    public messagesLoaded = false;
    public active = false;
}
