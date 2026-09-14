import { ApplicationConfig, provideZonelessChangeDetection } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { provideIcons } from '@ng-icons/core';
import {
  heroPlus, heroTrash, heroPencil, heroArrowPath,
  heroExclamationCircle, heroCheckCircle, heroXCircle,
  heroHashtag, heroCube, heroQueueList,
  heroMagnifyingGlass, heroChevronUp, heroChevronDown, heroChevronUpDown,
  heroChevronLeft, heroChevronRight
} from '@ng-icons/heroicons/outline';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(),
    provideHttpClient(),
    provideRouter([]),
    provideIcons({
      heroPlus, heroTrash, heroPencil, heroArrowPath,
      heroExclamationCircle, heroCheckCircle, heroXCircle,
      heroHashtag, heroCube, heroQueueList,
      heroMagnifyingGlass, heroChevronUp, heroChevronDown, heroChevronUpDown,
      heroChevronLeft, heroChevronRight
    })
  ]
};
