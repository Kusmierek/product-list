import { ApplicationConfig, provideZonelessChangeDetection } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideIcons } from '@ng-icons/core';
import {
  heroPlus, heroTrash, heroPencil, heroArrowPath,
  heroExclamationCircle, heroCheckCircle, heroXCircle,
  heroHashtag, heroCube, heroQueueList
} from '@ng-icons/heroicons/outline';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(),
    provideHttpClient(),
    provideIcons({
      heroPlus, heroTrash, heroPencil, heroArrowPath,
      heroExclamationCircle, heroCheckCircle, heroXCircle,
      heroHashtag, heroCube, heroQueueList
    })
  ]
};
